namespace APICatalogo.Pagination
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public bool HasPrevious => CurrentPage > 1;   //True quando pagina for maior que 1
        public bool HasNext => CurrentPage < TotalPages; //true quando for menor que o número de páginas

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {                    //itens da lista, quantidade de itens, número página e numero elementos
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize); //Calcula total de páginas (n° de itens dividido pelo tamanho da página)

            AddRange(items);  //Adiciona itens na lista
        }

        public static PagedList<T> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count(); //total de itens
            var items = source.Skip((pageNumber - 1) * pageSize) //Ignorar os registros da página anterior
                .Take(pageSize).ToList();  //seleciona  os registros da página

            return new PagedList<T>(items, count, pageNumber, pageSize); //Retorna PageList com os dados
        }
    }
}
