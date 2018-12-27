using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Athenas.Domain
{
    public static class ErrorBase
    {
        public const string erro_min = "Caracteres não atingiram o mínimo necessário";
        public const string erro_max = "Caracteres ultrapassaram o máximo permitido";
        public const string erro_for = "Formato inválido";
        public const string erro_nec = "Campo necessário";

        // Salt
        public const string global_salt = "SpaceNeedleAthenasSenaiInfoSP";
    }
}
