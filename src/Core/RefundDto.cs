namespace Core;

public class RefundDto
{
    public string Customer { get; set; }
    public Guid OrderId { get; set; }
    public float Amount { get; set; }
    public DateTime CreatedAt { get; set; }
}