namespace NL_THUD.Dtos.Request
{
    public class ClassRequest
    {
        public string Classes_Name { get; set; }
        public string? Classes_Code { get; set; }
        public string? Classes_Quantity { get; set; }
        public string Year { get; set; }
        public string? Semester { get; set; }
        public Guid Teacher_Id { get; set; }
    }
}
