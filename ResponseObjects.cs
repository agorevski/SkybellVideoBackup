using System;
using System.Collections.Generic;

namespace BackupVideos
{
    public class LoginResponse
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string resourceId { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public string id { get; set; }
        public List<object> userLinks { get; set; }
        public string access_token { get; set; }
    }

    public class Device
    {
        public string uuid { get; set; }
        public string resourceId { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string status { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public string id { get; set; }
    }

    public class Owner
    {
        public string _id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string resourceId { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }

    public class SubscriptionInfo
    {
        public Device device { get; set; }
        public string user { get; set; }
        public Owner owner { get; set; }
        public string acl { get; set; }
        public string id { get; set; }
    }










    public class Dataset
    {
        public int count { get; set; }
        public int pages { get; set; }
        public int returnedCount { get; set; }
        public int offset { get; set; }
    }

    public class Sort
    {
        public int createdAt { get; set; }
    }

    public class Meta
    {
        public Dataset dataset { get; set; }
        public Sort sort { get; set; }
        public object filter { get; set; }
    }

    public class Links
    {
        public string self { get; set; }
        public object first { get; set; }
        public object last { get; set; }
        public object prev { get; set; }
        public object next { get; set; }
    }

    public class Datum
    {
        public string _id { get; set; }
        public DateTime updatedAt { get; set; }
        public DateTime createdAt { get; set; }
        public string device { get; set; }
        public string callId { get; set; }
        public string @event { get; set; }
        public string state { get; set; }
        public DateTime ttlStartDate { get; set; }
        public string videoState { get; set; }
        public int __v { get; set; }
        public string id { get; set; }
        public string media { get; set; }
        public string mediaSmall { get; set; }
    }

    public class Activities
    {
        public Meta meta { get; set; }
        public Links links { get; set; }
        public List<Datum> data { get; set; }
    }

    public class Video
    {
        public string url { get; set; }
    }

}