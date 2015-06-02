using System.Data;

namespace Seaman.Core
{
    public interface ISeamanConnection
    {
        IDbConnection GetConnection();
    }
}