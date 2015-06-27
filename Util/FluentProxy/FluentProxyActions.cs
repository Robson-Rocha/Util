namespace RobsonROX.Util.FluentProxy
{
    internal class FluentProxyActions<T>
    {
        internal BeforeDelegate<T> BeforeActions { get; set; }
        internal InsteadOfDelegate<T> InsteadOfActions { get; set; }
        internal AfterDelegate<T> AfterActions { get; set; }
    }
}