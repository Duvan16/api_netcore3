using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_netcore3.Services
{
    public interface IRepositorioAutores
    {
        Autor ObtenerPorId(int id);
    }
}
