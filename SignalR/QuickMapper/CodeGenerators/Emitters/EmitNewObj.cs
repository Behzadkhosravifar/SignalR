﻿using System;
using System.Reflection;
using Express.ObjectMapper.Core.Extensions;

namespace Express.ObjectMapper.CodeGenerators.Emitters
{
    internal sealed class EmitNewObj : IEmitterType
    {
        private EmitNewObj(Type objectType)
        {
            ObjectType = objectType;
        }

        public Type ObjectType { get; private set; }

        public void Emit(CodeGenerator generator)
        {
            ConstructorInfo ctor = ObjectType.GetDefaultCtor();
            generator.EmitNewObject(ctor);
        }

        public static IEmitterType NewObj(Type objectType)
        {
            return new EmitNewObj(objectType);
        }
    }
}
