// See https://aka.ms/new-console-template for more information

using BreakingChanges;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using NStore.Persistence.Mongo;

var objectSerializer = new ObjectSerializer(type => ObjectSerializer.AllAllowedTypes(type));
BsonSerializer.RegisterSerializer(objectSerializer);

var conn = new MongoUrl("mongodb://admin:123456##@mongo01.codewrecks.com/testdriver?authSource=admin&compressors=snappy");
var client = new MongoClient(conn);
var db = client.GetDatabase("test");

MongoPersistenceOptions options = new MongoPersistenceOptions();
options.PartitionsCollectionName = "partitions";
options.PartitionsConnectionString = conn.ToString();

//this does not compile because of strongly typed mongodb driver
//options.CreateClientFunction = (_) => client;

//You can survive with reflection, calling method with reflection works
//because .NET 6 and greater automatically resolve the latest version of the references.
var property = options.GetType().GetProperty("CreateClientFunction")!;
property.SetValue(options, (MongoClientSettings _) => client);

// Use the library and verify that everything is ok.
var persistence = new MongoPersistence(options);
await persistence.InitAsync(CancellationToken.None);

SamplePayload samplePayload = new SamplePayload();
samplePayload.MyProperty = "Hello, World!";
samplePayload.IntProperty = 42;

await persistence.AppendAsync(Guid.NewGuid().ToString(), 1, samplePayload, Guid.NewGuid().ToString(), CancellationToken.None);

//var someContainerAdapter = new BuiltinHandlerActivator();
//var config = Rebus.Config.Configure
//    .With(someContainerAdapter)
//    .Subscriptions(s => s.StoreInMongoDb(db, "TEST"));

Console.WriteLine("Hello, World!");
