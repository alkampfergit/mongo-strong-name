// See https://aka.ms/new-console-template for more information

using MongoDB.Driver;
using Rebus.Activation;
using Rebus.Config;

var conn = new MongoUrl("mongodb://localhost:27017");
var client = new MongoClient(conn);
var db = client.GetDatabase("test");

var someContainerAdapter = new BuiltinHandlerActivator();
var config = Rebus.Config.Configure
    .With(someContainerAdapter)
    .Subscriptions(s => s.StoreInMongoDb(db, "TEST"));
Console.WriteLine("Hello, World!");
