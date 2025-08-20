using Datos;

public class ProductoNegocio
{
    private readonly ProductoDatos _productoDatos = new ProductoDatos();

    public List<Dictionary<string, object>> ObtenerProductos()
    {
        return _productoDatos.ObtenerProductos();
    }
}
