// Models

export default class RawRankingModel {
    constructor(
        public authorityId: number,
        public bookId: number,
        public commentaryId: number,
        public rank: number,
    ) { }
}
