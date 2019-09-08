import HttpService, { HttpResponse, HttpError } from '@/services/HttpService';
import BookInfoModel from '@/models/BookInfo';

export default class BookInfoService {
    public static getBookInfo(isbn: string): Promise<BookInfoModel | null | undefined> {
        const params: Map<string, string> = new Map();
        params.set('bibkeys', `ISBN:${isbn}`);
        params.set('jscmd', 'data');
        params.set('format', 'json');
        return HttpService.get('https://openlibrary.org/api/books', false, params)
            .then((response: HttpResponse<any>) => {
                for (const identifier of Object.keys(response.data)) {
                    const GOODREADS_KEY = 'goodreads';
                    const bookInfo: any = response.data[identifier];
                    const identifiers: string[] = bookInfo.identifiers[GOODREADS_KEY];

                    if (!identifiers) {
                        return null;
                    }
                    const imageUrl = bookInfo.cover ? bookInfo.cover.medium : '';
                    const openLibraryUrl = bookInfo.url;
                    const title = bookInfo.title;
                    const author = bookInfo.authors ? bookInfo.authors[0].name : 'N/A';
                    const urls = BookInfoService.constructGoodreadsUrls(identifiers);
                    return new BookInfoModel(
                        imageUrl,
                        openLibraryUrl,
                        title,
                        urls,
                        author,
                        isbn,
                    );
                }
            })
            .catch((error: HttpError) => {
                console.error('Could not get commentary info', error);
                return null;
            });
    }

    private static constructGoodreadsUrls(ids: string[]) {
        const urls: string[] = ids.map((id: string) => {
            return `https://www.goodreads.com/book/show/${id}`;
        });
        return urls;
    }
}
