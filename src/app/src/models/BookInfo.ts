export default class BookInfoModel {
    constructor(
        public imageUrl: string,
        public openLibraryUrl: string,
        public title: string,
        public urls: string[],
        public author: string,
        public isbn: string,
    ) { }
}
