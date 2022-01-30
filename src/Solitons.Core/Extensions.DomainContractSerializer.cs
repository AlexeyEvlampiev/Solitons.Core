using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Solitons
{
    public static partial class Extensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <param name="dto"></param>
        /// <param name="argumentName"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static IDomainContractSerializer ThrowIfNotSupportedDtoArgument(this IDomainContractSerializer self,
            object dto,
            string argumentName,
            out string contentType)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            var type = dto.GetType();
            if (self.CanSerialize(dto, out contentType))
                return self;
            var message = new StringBuilder("The give Data Transfer Object type is not supported.")
                .Append($" Argument type: {dto.GetType()}.");

            if (Attribute.GetCustomAttribute(type, typeof(GuidAttribute)) is null)
            {
                message.Append($" Did you forget annotating this type with {typeof(GuidAttribute)}?");
            }

            if (Attribute.GetCustomAttribute(type, typeof(DataTransferObjectAttribute)) is null ||
                typeof(IBasicJsonDataTransferObject).IsAssignableFrom(type) == false ||
                typeof(IBasicXmlDataTransferObject).IsAssignableFrom(type) == false)
            {
                message
                    .Append($" Did you forget annotating this type with {typeof(DataTransferObjectAttribute)}?")
                    .Append($" Alternatively you can make {type} type implementing {typeof(IBasicJsonDataTransferObject)} or {typeof(IBasicXmlDataTransferObject)} interface.");
            }

            throw new ArgumentException(message.ToString(), argumentName);
        }
    }
}
