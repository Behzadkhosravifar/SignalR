﻿using System;
using System.Linq.Expressions;

namespace Express.ObjectMapper.Bindings
{
    public interface IBindingConfig<TSource, TTarget>
    {
        void Bind(Expression<Func<TSource, object>> source, Expression<Func<TTarget, object>> target);
        void Ignore(Expression<Func<TSource, object>> expression);
    }
}
