using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LeafLetMongo.Models
{
    public class GeoPointArray
    {
        public string type { get; set; }
        public double[][][][] coordinates { get; set; }
    }
    public class GeoPointBson
    {
        public string type { get; set; }
        public BsonArray coordinates { get; set; }
    }

    public class Polygon
    {
        public GeoPointArray Geo { get; set; }
        public string Name { get; set; }
        public string Mahalle { get; set; }
    }
    public class PolygonOutput
    {
        public GeoPointBson Geo { get; set;}
        public string Name { get; set; }
        public string Mahalle { get; set; }
    }
}