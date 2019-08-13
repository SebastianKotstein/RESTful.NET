using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.net.http.jsonkit
{
    public interface ISchemaHandler
    {
        /// <summary>
        /// Returns true if the <see cref="Type"/> having the passed name (case-insesitive) is registered, else false.
        /// </summary>
        /// <param name="typeName">name of the <see cref="Type"/> (case-insensitive)</param>
        /// <returns>true if registered, else false</returns>
        bool ContainsType(string typeName);

        /// <summary>
        /// Registers a new <see cref="Type"/> under the passed type name (case-insensitive). The methods throws an <see cref="Exception"/> if the type name is already used for another <see cref="Type"/> registration.
        /// The methods returns the type name converted to lowercase (i.e. which is effectively used for registration).
        /// </summary>
        /// <param name="typeName">name of the <see cref="Type"/> (case-insensitive)</param>
        /// <param name="type"><see cref="Type"/> which should be registered</param>
        /// <returns>type name converted to lowercase</returns>
        string RegisterType(string typeName, Type type);

        /// <summary>
        /// Registers a new <see cref="Type"/>. The <see cref="Type.FullName"/> is used as the name of the <see cref="Type"/> (converted to lowercase). The methods throws an <see cref="Exception"/> if the type name is already used for  another <see cref="Type"/> registration.
        /// The methods returns the type name converted to lowercase (i.e. which is effectively used for registration).
        /// </summary>
        /// <param name="type"><see cref="Type"/> which should be registered</param>
        /// <returns>type name converted to lowercase</returns>
        string RegisterType(Type type);

        /// <summary>
        /// Registers a new <see cref="Type"/> under the passed type name (case-insensitive). The methods throws an <see cref="Exception"/> if the type name is already used for  another <see cref="Type"/> registration.
        /// The methods returns the type name converted to lowercase (i.e. which is effectively used for registration).
        /// </summary>
        /// <param name="typeName">name of the <see cref="Type"/> (case-insensitive)</param>
        /// <param name="type"></param>
        /// <param name="modifier"><see cref="ISchemaModifier"/> modifying the <see cref="JSchema"/> of the registered type before returning (see <see cref="ISchemaModifier"/> and <see cref="GetSchema(string)"/>)</param>
        /// <returns>type name converted to lowercase</returns>
        string RegisterType(string typeName, Type type, ISchemaModifier modifier);

        /// <summary>
        /// Registers a new <see cref="Type"/>. The <see cref="Type.FullName"/> is used as the name of the <see cref="Type"/> (converted to lowercase). The methods throws an <see cref="Exception"/> if the type name is already used for  another <see cref="Type"/> registration.
        /// The methods returns the type name converted to lowercase (i.e. which is effectively used for registration).
        /// </summary>
        /// <param name="type"><see cref="Type"/> which should be registered</param>
        /// <param name="modifier"><see cref="ISchemaModifier"/> modifying the <see cref="JSchema"/> of the registered type before returning (see <see cref="ISchemaModifier"/> and <see cref="GetSchema(string)"/>)</param>
        /// <returns>type name converted to lowercase</returns>
        string RegisterType(Type type, ISchemaModifier modifier);

        /// <summary>
        /// Unregisters the <see cref="Type"/> having the passed name (case-insensitive). If the specified <see cref="Type"/> is unknown or has been unregistered before, nothing will happen.
        /// </summary>
        /// <param name="typeName"><see cref="Type"/> which should be removed</param>
        void UnregisterType(string typeName);

        /// <summary>
        /// Returns the <see cref="Type"/> having the passed name (case insensitive). The method throws an <see cref="Exception"/> if the <see cref="Type"/> is not registered.
        /// </summary>
        /// <param name="typeName">name of the <see cref="Type"/> (case-insensitive)</param>
        /// <returns><see cref="Type"/> having the passed name (case insensitive)</returns>
        Type GetRegisteredType(string typeName);

        /// <summary>
        /// Returns the <see cref="JSchema"/> of the registered <see cref="Type"/> having the passed name.
        /// If an <see cref="ISchemaModifier"/> is registered with the requested <see cref="Type"/>, the <see cref="JSchema"/> will be modified by this implementation of <see cref="ISchemaModifier"/>.
        /// The methods throws a <see cref="ResourceNotFoundException"/> if the type name is unknown (i.e. not registered) and an <see cref="Exception"/> if the <see cref="JSchema"/> cannot be created.
        /// </summary>
        /// <param name="typeName">>name of the <see cref="Type"/> (case-insensitive)</param>
        /// <returns><see cref="JSchema"/> of the registered <see cref="Type"/></returns>
        JSchema GetSchema(string typeName);

        
    }
}
