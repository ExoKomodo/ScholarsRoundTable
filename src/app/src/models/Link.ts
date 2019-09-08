// const LOREM_IPSUM: string = `
// Lorem ipsum dolor sit amet, consectetur adipiscing elit. Fusce lobortis eu dolor eget malesuada.
// Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus.
// Proin ut lacus sit amet magna tincidunt sollicitudin.
// `;

export default class LinkModel {
    constructor(
        public route: string,
        public text: string = '',
        public content: string = '',
    ) {
        this.content = content;
    }
}
