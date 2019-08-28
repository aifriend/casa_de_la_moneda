
namespace RCMCommonTypes
{

    [System.Serializable()]
    public class Country
    {
        public string Name { get; private set; }
        public int Code { get; private set; }
        
        public static readonly Country España = new Country("España", 1);
        public static readonly Country Panama = new Country("Panama", 2);
        public static readonly Country RepDominicana = new Country("Rep. Dominicana", 3);

        public static Country GetCountry(int countryCode)
        {
            Country value = null;
            switch (countryCode)
            {
                case 1:
                    value = España;
                    break;
                case 2:
                    value = Panama;
                    break;
                case 3:
                    value = RepDominicana;
                    break;
                default:
                    value = null;
                    break;
            }
            return value;
        }

        public Country(string name, int code)
        {
            Name = name;
            Code = code;
        }
        public override string ToString()
        {
            return Name;
        }
    }
}