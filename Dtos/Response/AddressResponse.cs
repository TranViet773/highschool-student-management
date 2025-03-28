namespace NL_THUD.Dtos.Response
{
    public class AddressResponse
    {
        public string Address_Detail { get; set; }  
        public int Ward_Id { get; set; }    
        public string Ward_Name { get; set; }
        public int District_Id { get; set; }
        public string District_Name { get;set; }
        public int Province_Id { get; set; }
        public string Province_Name { get; set; }

    }
}
