namespace NL_THUD.Models
{
    public class Teacher_Class
    {
        public string Teacher_Id { get; set; }
        public Teacher Teacher { get; set; }

        public Guid Class_Id { get; set; }
        public Classes Class { get; set; }
        public string Year { get; set; }
        public string Semester { get; set; }
    }
}
