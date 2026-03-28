
public interface IEnableable
{
    public bool Enabled { get; set; }  
   
    public void Enable();
    public void Disable();
}