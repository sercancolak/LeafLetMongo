using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using LeafLetMongo.Models;
using System.Threading.Tasks;
namespace LeafLetMongo.Controllers
{
    public class MongoController : ApiController
    {
        MongoClient client = new MongoClient();
        [HttpPost]
        public async Task<PolygonOutput> Insert (Polygon koordinat)
        {
            var result = new PolygonOutput();
            try
            {
                var db = client.GetDatabase("Sehirler");
                var collection = db.GetCollection<BsonDocument>("Mahalleler");

                if(koordinat.Geo.type == "Polygon")
                {
                    var gonder = new PolygonOutput()
                    {
                        Mahalle = koordinat.Mahalle,
                        Name = koordinat.Name,
                        Geo = new GeoPointBson()
                        {
                            coordinates = BsonArray.Create(koordinat.Geo.coordinates[0]),
                            type = koordinat.Geo.type
                        }
                    };
                   await collection.InsertOneAsync(gonder.ToBsonDocument());
                }
                else if(koordinat.Geo.type == "MultiPolygon")
                {
                    var gonder = new PolygonOutput()
                    {
                        Mahalle = koordinat.Mahalle,
                        Name = koordinat.Name,
                        Geo = new GeoPointBson()
                        {
                            coordinates = BsonArray.Create(koordinat.Geo.coordinates),
                            type = koordinat.Geo.type
                        }
                    };
                   await collection.InsertOneAsync(gonder.ToBsonDocument());
                }
            }
            catch(Exception ex)
            {
            }
            return result;
        }
        [HttpGet]
        public async Task<PolygonOutput> Goster(string Mahalle)
        {

            var db = client.GetDatabase("Sehirler");
            var collection = db.GetCollection<BsonDocument>("Mahalleler");
            var filter = Builders<BsonDocument>.Filter.Eq("Mahalle", Mahalle);
            var result = collection.Find(filter).FirstOrDefault();
            var output = new PolygonOutput()
            {
                Geo = new GeoPointBson()
                {
                    type = result["Geo"]["type"].AsString,
                    coordinates = result["Geo"]["coordinates"].AsBsonArray
                },
                Name = result["Name"].AsString,
                Mahalle = result["Mahalle"].AsString
            };
            return output;
        }
        [HttpPost]
        public async Task<List<PolygonOutput>> ComboDoldur(Polygon Mahalle)
        {
            var db = client.GetDatabase("Sehirler");
            var collection = db.GetCollection<BsonDocument>("Mahalleler");
            var filter = Builders<BsonDocument>.Filter.Ne("Mahalle", "");
            var output = new List<PolygonOutput>();
            //var result = collection.Find(filter).FirstOrDefault();
            var result = collection.Find(filter).ToList();
            if (result.Count > 0)
            {
                foreach (var item in result)
                {
                    var gelen = new PolygonOutput()
                    {
                        Mahalle = item["Mahalle"].AsString
                    };
                    output.Add(gelen);
                }
            }
         
                return output;
           

        }
        [HttpPost]
        public async Task<PolygonOutput> Update(Polygon koordinat)
        {
            var result = new PolygonOutput();
            try
            {
                var db = client.GetDatabase("Sehirler");
                var collection = db.GetCollection<BsonDocument>("Mahalleler");
                if (koordinat.Geo.type == "Polygon")
                {
                    var gonder = new PolygonOutput()
                    {
                        Mahalle = koordinat.Mahalle,
                        Name = koordinat.Name,
                        Geo = new GeoPointBson()
                        {
                            coordinates = BsonArray.Create(koordinat.Geo.coordinates[0]),
                            type = koordinat.Geo.type
                        }
                    };
                    var filter = Builders<BsonDocument>.Filter.Eq("Mahalle", koordinat.Mahalle);
                    var update = Builders<BsonDocument>.Update.Set("Geo.coordinates", gonder.Geo.coordinates).Set("Geo.type",gonder.Geo.type);
                    await collection.UpdateOneAsync(filter,update);
                }
                else if (koordinat.Geo.type == "MultiPolygon")
                {
                    var gonder = new PolygonOutput()
                    {
                        Mahalle = koordinat.Mahalle,
                        Name = koordinat.Name,
                        Geo = new GeoPointBson()
                        {
                            coordinates = BsonArray.Create(koordinat.Geo.coordinates),
                            type = koordinat.Geo.type
                        }
                    };
                    var filter = Builders<BsonDocument>.Filter.Eq("Mahalle", koordinat.Mahalle);
                    var update = Builders<BsonDocument>.Update.Set("Geo.coordinates", gonder.Geo.coordinates).Set("Geo.type", gonder.Geo.type);
                    await collection.UpdateOneAsync(filter, update);
                }
            }
            catch (Exception ex)
            {
            }
            return result;
        }
    }
}
