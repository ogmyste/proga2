using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Data
{
    /// <summary>
    /// Baasklass kõikidele klassidele, mille jaoks on
    /// ApplicationDbContextis oma DbSet
    /// </summary>
[ExcludeFromCodeCoverage]
    public abstract class Entity
    {
        public int Id { get; set; }
    }
}
