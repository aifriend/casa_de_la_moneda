using System;
using System.IO;
using System.Linq;
using Idpsa.Control.Tool;

namespace Idpsa.Control.Component
{
    [Serializable]
    public class Profibus : Bus
    {
        public Profibus(int efectiveInputLeght, int efectiveOutputLength)
            : base(efectiveInputLeght, efectiveOutputLength){}

        protected override void LoadSimbolic(int efectiveInputLength, int efectiveOutputLength)
        {
            using (var reader = new StreamReader(ConfigFiles.Simbolic))
            {
                foreach (var data in 
                    (from line in reader.Lines()
                     where (line.StartsWith("I") || line.StartsWith("O"))
                     let parts = line.Substring(1).Split(';')
                     select new
                                {
                                    IsInput = line.StartsWith("I"),
                                    Symbol = parts[1],
                                    Description = parts[2],
                                    IoSignal = new IOSignal {Name = parts[1]},
                                    Address = new Address(int.Parse(parts[0].Split('.')[0]),
                                                          int.Parse(parts[0].Split('.')[1]))
                                }))
                    if (data.IsInput)
                    {
                        AddInput(new Input
                                     {
                                         Address = data.Address,
                                         Symbol = data.Symbol,
                                         Description = data.Description,
                                         IOSignal = data.IoSignal
                                     });
                    }
                    else
                    {
                        AddOutput(new Output
                                      {
                                          Address = data.Address,
                                          Symbol = data.Symbol,
                                          Description = data.Description,
                                          IOSignal = data.IoSignal
                                      });
                    }
            }
        }
        protected override void CreateController()
        {
            Controller = new ProfibusController(0, InCollection, OutCollection, 0);
        }
    }
}