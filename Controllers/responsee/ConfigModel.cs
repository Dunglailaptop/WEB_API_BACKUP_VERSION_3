using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyCinema.Model
{
    public class ConfigModel{

      [MaxLength(50)]
      public string username {get; set;}
     
      public int ProjectId {get; set;}
      
    }
    

}