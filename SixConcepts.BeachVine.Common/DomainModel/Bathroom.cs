using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixConcepts.BeachVine.DomainModel
{
  public class Bathroom
  {
    public IEnumerable<Image> Images
    {
      get;
      set;
    }
  }
}
