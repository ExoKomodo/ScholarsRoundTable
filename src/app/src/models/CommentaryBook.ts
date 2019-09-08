export default class CommentaryBookModel {
    constructor(
        public bookId: number,
        public bookName: string,
        public commentaryId: number,
        public commentaryName: string,
    ) { }
}
