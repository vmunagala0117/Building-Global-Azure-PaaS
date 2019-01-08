using AZ_Paas_Demo.Data.Interfaces;
using AZ_Paas_Demo.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AZ_Paas_Demo.Data.Services
{
    public class JuiceService : IJuiceService
    {
        private azpaasdemodbContext _context;
        public JuiceService(azpaasdemodbContext context)
        {
            _context = context;
        }
        public List<Juices> GetAllJuices()
        {
            List<Juices> juices = _context.Juices.ToList();
            return juices;
        }       
    }
}
