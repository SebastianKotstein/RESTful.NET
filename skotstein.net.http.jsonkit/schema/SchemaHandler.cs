using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Schema;
using SKotstein.Net.Http.Context;
using SKotstein.Net.Http.Model.Exceptions;

namespace skotstein.net.http.jsonkit
{
    public class SchemaHandler : ISchemaHandler
    {
        private IDictionary<string, Type> _registeredTypes = new Dictionary<string, Type>();
        private IDictionary<string, ISchemaModifier> _modifiers = new Dictionary<string, ISchemaModifier>();

        public bool ContainsType(string typeName)
        {
            return _registeredTypes.ContainsKey(typeName.ToLower());
        }

        public Type GetRegisteredType(string typeName)
        {
            if (ContainsType(typeName))
            {
                return _registeredTypes[typeName.ToLower()];
            }
            else
            {
                throw new Exception("The type '" + typeName.ToLower() + "' is not registered");
            }
        }


        public string RegisterType(string typeName, Type type)
        {
            if (ContainsType(typeName.ToLower()))
            {
                throw new Exception("The type '" + typeName.ToLower() + "' is already registered");
            }
            else
            {
                _registeredTypes[typeName.ToLower()] = type;
                return typeName.ToLower();
            }
        }

        public string RegisterType(Type type)
        {
            if (ContainsType(type.Name.ToLower()))
            {
                throw new Exception("The type '" + type.Name.ToLower() + "' is already registered");
            }
            else
            {
                _registeredTypes[type.Name.ToLower()] = type;
                return type.Name.ToLower();
            }
        }


        public string RegisterType(string typeName, Type type, ISchemaModifier modifier)
        {
            string tName = RegisterType(typeName, type);
            if(modifier != null)
            {
                _modifiers[tName] = modifier;
            }
            return tName;
        }

        public string RegisterType(Type type, ISchemaModifier modifier)
        {
            string tName = RegisterType(type);
            if (modifier != null)
            {
                _modifiers[tName] = modifier;
            }
            return tName;
        }

        public void UnregisterType(string typeName)
        {
            if (ContainsType(typeName.ToLower()))
            {
                _registeredTypes.Remove(typeName.ToLower());
                if (_modifiers.ContainsKey(typeName.ToLower()))
                {
                    _modifiers.Remove(typeName.ToLower());
                }
            }
        }

        public JSchema GetSchema(string typeName)
        {
            typeName = typeName.ToLower();

            if (ContainsType(typeName))
            {
                JSchema jSchema = JsonSchemaGenerator.Generate(GetRegisteredType(typeName));
                if(jSchema == null)
                {
                    throw new HttpRequestException("Cannot create schema for '" + _registeredTypes[typeName].FullName + "'", MimeType.TEXT_PLAN) { Status = HttpStatus.InternalServerError};
                }
                if (_modifiers.ContainsKey(typeName))
                {
                    _modifiers[typeName].Modify(jSchema);
                }
                return jSchema;
            }
            else
            {
                throw new ResourceNotFoundException("The schema '" + typeName + "' is unknown");
            }
        }

    }
}
