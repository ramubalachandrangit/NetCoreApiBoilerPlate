using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Converters
{
    public interface IConvertModel<TSource, TTarget>
    {
        TTarget Convert();       
    }
}
