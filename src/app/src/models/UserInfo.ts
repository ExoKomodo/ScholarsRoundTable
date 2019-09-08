export default class UserInfoModel {
    constructor(
        public email: string,
        public isAdmin: boolean,
        public name: string,
        public profilePic: string,
        public id: string,
    ) { }
}
