// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializableAttribute.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#region license

// OxyPlot:   http://oxyplot.codeplex.com
// License:   Ms-PL
#endregion

namespace OxyPlot
{
    using System;

#if SILVERLIGHT || PCL

    /// <summary>
    /// Serialize attribute.
    /// </summary>
    public class SerializableAttribute : Attribute
    {
    }
#endif
}