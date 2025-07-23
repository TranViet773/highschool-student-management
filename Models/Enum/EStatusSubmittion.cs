namespace NL_THUD.Models.Enum
{
    public enum EStatusSubmittion
    {
        PENDING,        //0
        SUBMITTED,      //1
        COMPLETED,      //2
        REJECTED,       //3 
        CANCELED,       //4 
        EDITTING,       //5 - Giáo viên giảng dạy đang trong quá trình xem xét phúc khảo
        BLOCKED         //6
            //PENDING => SUBMITTED => EDITTING => BLOCKED => COMPLETED
            //PENDING => REJECTED
            //
    }
}
