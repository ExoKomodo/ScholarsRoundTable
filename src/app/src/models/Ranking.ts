// Models
import BookInfoModel from './BookInfo';

export default class RankingModel {
    constructor(
        public commentaryId: number,
        public commentaryName: string,
        public commentaryIsbn: string,
        public rank: number,
        public bookInfo: BookInfoModel | null | undefined = null,
    ) { }
}
