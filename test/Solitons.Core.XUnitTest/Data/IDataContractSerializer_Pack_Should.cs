using Xunit;

namespace Solitons.Data
{
    public class IDataContractSerializer_Pack_Should
    {
        [Fact]
        public void HandleCommandArgs()
        {
            var serializer = IDataContractSerializer
                .CreateBuilder()
                .Build();

            var args = ICommandArgs.CreateEmpty("449fb19b-b132-4ba2-acae-12bb5fa60c2e");
            var package = serializer.Pack(args);
            var clone = (ICommandArgs)serializer.Unpack(package, out var commandId);
            Assert.True(clone is CommandArgs);
            Assert.Equal(args.CommandId, commandId);
            Assert.Equal(args.CommandId, clone.CommandId);

        }
    }

}
