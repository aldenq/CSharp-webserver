namespace HTTP{






    public class Route{
        public int count;
        public Client_Handler.RouteHandler routeHandler;
        public Route(){

        }
    }


    public class HTTP_Config{
            private readonly Dictionary<string, Client_Handler.RouteHandler> _routeHandlers = new Dictionary<string, Client_Handler.RouteHandler>();

            public Client_Handler.RouteHandler notFound;

            public void RegisterRouteHandler(string route, Client_Handler.RouteHandler handler)
            {
                _routeHandlers[route] = handler;
            }


            public Client_Handler.RouteHandler GetRoute(string route){

                if (_routeHandlers.ContainsKey(route)){
                    return _routeHandlers[route];
                } else {
                    return this.notFound;
                }

            }



            public void RegisterNotFound(Client_Handler.RouteHandler handler){
                this.notFound = handler;


            }



        






            

    }





    
}