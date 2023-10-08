using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using Newtonsoft.Json;
using ProtoBuf;
using Solitons.Data;
using Solitons.Data.Common;

namespace UsageExamples.Data;

[Example]
public sealed class ExampleDefiningDataContracts
{
    public void Example()
    {
        // Construct an IDataContractSerializer instance for efficient and versatile data serialization.
        var serializer = IDataContractSerializer.Build(builder =>
        {
            // Register 'Person' type for serialization, supporting both XML and JSON formats.
            // Serialization precedence is determined by the order: the first serializer specified is the default.
            builder.Add(typeof(Person),
                new[] { IMediaTypeSerializer.BasicJsonSerializer, IMediaTypeSerializer.BasicXmlSerializer });

            // Register 'ProductEventData' type, specifying support for both ProtoBuf and JSON formats.
            // Serialization precedence: ProtoBuf is the default format.
            builder.Add(typeof(ProductEventData),
                new[] { ProtoBufMediaTypeSerializer.Instance, NewtonsoftMediaTypeSerializer.Instance });

            // Allow the serializer to process classes that are not explicitly annotated with a GUID attribute.
            // This can be beneficial when dealing with data contracts from external libraries that may not use GUID annotations.
            builder.IgnoreMissingCustomGuidAnnotation(true);

            // Auto-discover and register types from the entry assembly for serialization.
            builder.AddAssemblyTypes(Assembly.GetEntryAssembly()!, type =>
            {
                // Set criteria to determine which types should be serialized.
                // Types with names ending in 'Data' or 'Dto' are processed with both JSON and XML serializers.
                // Here, the JSON serializer takes precedence due to its position in the list.
                if (type.Name.EndsWith("Data") || type.Name.EndsWith("Dto"))
                {
                    return new[]
                    {
                        IMediaTypeSerializer.BasicJsonSerializer,
                        IMediaTypeSerializer.BasicXmlSerializer
                    };
                }

                // Exclude types that don't meet the naming criteria.
                return Enumerable.Empty<IMediaTypeSerializer>();
            });
        });

        var data = new ProductEventData
        {
            LaunchDate = DateTime.UtcNow,
            Price = 120.5M,
            ProductId = 305428,
            ProductName = "SodaX"
        };

        // Serialize 'ProductEventData' to its default format (ProtoBuf in this setup).
        var text = serializer.Serialize(data);

        // Assert that the serialization process has indeed used the ProtoBuf format.
        Debug.Assert(text.ContentType == "application/x-protobuf", "Expected format: ProtoBuf");
        Console.WriteLine($"ProtoBuf: {text}");

        // Serialize 'ProductEventData' explicitly specifying the JSON format.
        text = serializer.Serialize(data, "application/json");

        // Assert that the serialization process has used the specified JSON format.
        Debug.Assert(text.ContentType == "application/json", "Expected format: JSON");
        Console.WriteLine($"JSON: {text}");

        // Prepare 'ProductEventData' for queue transmission by packaging it with relevant metadata.
        var package = serializer.Pack(data);
        package.To = "product-events-queue";
        package.CorrelationId = Guid.NewGuid().ToString();

        // Note: Insert logic here to dispatch the package via a queue mechanism.

        // Unpack the serialized data for verification.
        var dataClone = serializer.Unpack(package);

        // Validate the deserialized object type to ensure data integrity.
        Debug.Assert(dataClone is ProductEventData, "Deserialized data type mismatch: Expected 'ProductEventData'");
    }


    // Example 1: Person represents a POCO data contract with rules for both JSON and XML formatting.
    // It's identified by the GUID 460d3673-c98e-406d-80d2-2e65862c6d87.
    // Register this contract when building the Solitons IDataContractSerializer instance using explicit or reflective methods.
    [Guid("460d3673-c98e-406d-80d2-2e65862c6d87")]
    public sealed class Person
    {
        [JsonPropertyName("firstName")]
        [XmlAttribute("FirstName")]
        public string FirstName { get; set; } = String.Empty;

        [JsonPropertyName("lastName")]
        [XmlAttribute("LastName")]
        public string LastName { get; set; } = String.Empty;

        [JsonPropertyName("age")]
        [XmlAttribute("Age")]
        public int Age { get; set; }
    }

    // Example 2: CustomerEngagementData is an "XML-first" data contract, deriving from BasicXmlDataTransferObject 
    // and implementing the IBasicJsonDataTransferObject Solitons interface.
    // When serialized with Solitons IDataContractSerializer, XML serialization is applied by default.
    // Data contracts inheriting from foundational classes are detected automatically through .NET reflection by IDataContractSerializer builders.
    // The base class provides convenient methods for direct serialization.
    // Identify this contract by the GUID c0d692ba-ecb6-44fa-9954-b31f15e03954 for Solitons management.
    [Guid("c0d692ba-ecb6-44fa-9954-b31f15e03954")]
    public sealed class CustomerEngagementData : BasicXmlDataTransferObject, IBasicJsonDataTransferObject
    {
        [JsonPropertyName("userEmail")]
        [XmlAttribute("UserEmail")]
        public string UserEmail { get; set; } = String.Empty;

        [JsonPropertyName("lastEngagedDate")]
        [XmlAttribute("LastEngagedDate")]
        public DateTime LastEngagedDate { get; set; }

        [JsonPropertyName("engagementScore")]
        [XmlAttribute("EngagementScore")]
        public double EngagementScore { get; set; }
    }

    // Example 3: UserRegistrationEventData is a "JSON-first" data contract.
    // It inherits from BasicJsonDataTransferObject and also implements the IBasicXmlDataTransferObject Solitons interface.
    // JSON serialization is the default behavior with Solitons IDataContractSerializer.
    // The base class provides methods for direct serialization without additional serializer components.
    // This contract has a unique GUID identifier a01619cd-9c93-4f8a-9cf3-f7b198c0769a for Solitons usage.
    [Guid("a01619cd-9c93-4f8a-9cf3-f7b198c0769a")]
    public sealed class UserRegistrationEventData : BasicJsonDataTransferObject, IBasicXmlDataTransferObject
    {
        [JsonPropertyName("username")]
        [XmlAttribute("Username")]
        public string Username { get; set; } = String.Empty;

        [JsonPropertyName("registrationDate")]
        [XmlAttribute("RegistrationDate")]
        public DateTime RegistrationDate { get; set; }

        [JsonPropertyName("referralCode")]
        [XmlAttribute("ReferralCode")]
        public string ReferralCode { get; set; } = String.Empty;
    }

    // Example 4: ProductEventData represents a data contract that uses both Newtonsoft's JSON library 
    // and Protocol Buffers (Protobuf) for serialization.
    // This class doesn't inherit from any Solitons-specific classes or interfaces. 
    // When using with HTTP protocols, the media type for Protobuf is "application/x-protobuf".
    // To use this contract within Solitons, you must provide corresponding implementations of IMediaTypeSerializer 
    // for both the Newtonsoft JSON and Protobuf serialization formats.
    [Guid("f2c63f9b-1e5a-4b8a-8d45-65a2f245d7ed")]
    [ProtoContract]
    public sealed class ProductEventData
    {
        [JsonProperty(PropertyName = "productId")]// Newtonsoft JSON formatting
        [ProtoMember(1)]// Protobuf formatting
        public int ProductId { get; set; }

        [JsonProperty(PropertyName = "productName")]// Newtonsoft JSON formatting
        [ProtoMember(2)]// Protobuf formatting
        public string ProductName { get; set; } = String.Empty;

        [JsonProperty(PropertyName = "launchDate")]// Newtonsoft JSON formatting
        [ProtoMember(3)]// Protobuf formatting
        public DateTime LaunchDate { get; set; }

        [JsonProperty(PropertyName = "price")]// Newtonsoft JSON formatting
        [ProtoMember(4)]// Protobuf formatting
        public decimal Price { get; set; }
    }

    // Example 5: NewtonsoftMediaTypeSerializer
    // This class provides a concrete implementation of the MediaTypeSerializer for JSON serialization using the Newtonsoft library.
    // This serializer targets the "application/json" media type, which is a standard MIME type for JSON data.
    // Solitons' IDataContractSerializer implementations leverage this class to handle serialization of classes that rely on Newtonsoft's JSON format.
    public sealed class NewtonsoftMediaTypeSerializer : MediaTypeSerializer
    {
        // Provides a static instance of the NewtonsoftMediaTypeSerializer to be used wherever needed.
        // This follows a Singleton pattern, ensuring a single shared instance across the application.
        public static readonly IMediaTypeSerializer Instance = new NewtonsoftMediaTypeSerializer();

        // Private constructor ensures that no other instances can be created directly.
        // The base constructor is called with "application/json", setting the media type for this serializer.
        private NewtonsoftMediaTypeSerializer()
            : base("application/json")
        {
        }

        // Serializes the provided data transfer object (dto) to its JSON representation using Newtonsoft.
        // The result is formatted with indents for better readability.
        protected override string Serialize(object dto) => JsonConvert.SerializeObject(dto, Formatting.Indented);

        // Deserializes the provided JSON content to an object of the specified targetType using Newtonsoft.
        // Returns the deserialized object, or null if the content doesn't match the target type.
        protected override object? Deserialize(string content, Type targetType) => JsonConvert.DeserializeObject(content, targetType);
    }



    // Example 6: ProtoBufMediaTypeSerializer
    // This class provides a concrete implementation of the MediaTypeSerializer for serialization using the Protocol Buffers (ProtoBuf) format.
    // This serializer targets the "application/x-protobuf" media type, which is a common MIME type for Protocol Buffers data.
    // Solitons' IDataContractSerializer implementations can leverage this class to handle serialization of classes that rely on ProtoBuf's format.
    public sealed class ProtoBufMediaTypeSerializer : MediaTypeSerializer
    {
        // Provides a static instance of the ProtoBufMediaTypeSerializer to be used wherever needed.
        // This follows a Singleton pattern, ensuring a single shared instance across the application.
        public static readonly IMediaTypeSerializer Instance = new ProtoBufMediaTypeSerializer();

        // Private constructor ensures that no other instances can be created directly.
        // The base constructor is called with "application/x-protobuf", setting the media type for this serializer.
        private ProtoBufMediaTypeSerializer()
            : base("application/x-protobuf")
        {
        }

        // Serializes the provided data transfer object (dto) into its Protocol Buffers binary representation using the protobuf-net library.
        protected override string Serialize(object dto)
        {
            using var stream = new System.IO.MemoryStream();
            Serializer.Serialize(stream, dto);
            return Convert.ToBase64String(stream.ToArray()); // For simplicity, converting the binary to Base64 string. Adjust if needed.
        }

        // Deserializes the provided Protocol Buffers content to an object of the specified targetType using the protobuf-net library.
        // Returns the deserialized object, or null if the content doesn't match the target type.
        protected override object? Deserialize(string content, Type targetType)
        {
            var bytes = Convert.FromBase64String(content); // Convert Base64 string back to binary. Adjust if needed.
            using var stream = new System.IO.MemoryStream(bytes);
            return Serializer.Deserialize(targetType, stream);
        }
    }

}