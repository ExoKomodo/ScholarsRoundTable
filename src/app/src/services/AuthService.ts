import HttpService, { HttpResponse, HttpError } from '@/services/HttpService';
import UserInfoModel from '@/models/UserInfo';
import VueRouter from 'vue-router';

export default class AuthService {
    public static get userInfo(): UserInfoModel | null {
        let userInfoModel: UserInfoModel | null = null;
        if (localStorage.userInfo) {
            userInfoModel = JSON.parse(localStorage.userInfo);
        }
        return userInfoModel;
    }

    public static set userInfo(value: UserInfoModel | null) {
        if (value == null) {
            AuthService.removeUserInfo();
        } else {
            localStorage.userInfo = JSON.stringify({
                email: value.email,
                isAdmin: value.isAdmin,
                name: value.name,
                profilePic: value.profilePic,
                id: value.id,
            });
        }
    }

    public static rerouteNonAdminUsers(router: VueRouter): void {
        if (this.userInfo == null || !this.userInfo.isAdmin) {
            const url = window.location.pathname;
            const splitUrl = url.split('/');
            splitUrl.shift();
            switch (splitUrl[0]) {
                case 'home':
                case 'faq':
                case 'search':
                    return;
                case 'commentaries':
                    if (splitUrl.length > 1) {
                        return;
                    }
                default:
                    router.push('');
            }
          }
    }

    public static isLoggedIn(): boolean {
        return this.userInfo != null;
    }

    public static login(): void {
        window.location.replace(`${HttpService.baseAuthUrl}/auth/login`);
    }

    public static logout(): void {
        AuthService.removeUserInfo();
        window.location.replace(`${HttpService.baseAuthUrl}/auth/logout`);
    }

    public static setUserInfo(userId: string): Promise<any> {
        const params: Map<string, string> = new Map();
        params.set('userId', userId);
        return HttpService.get(`${HttpService.baseAuthUrl}/auth/user-info`, false, params)
            .then((response: HttpResponse<UserInfoModel>) => {
                this.userInfo = response.data;
            })
            .catch((error: HttpError) => {
                console.error(error);
                this.userInfo = null;
            });
    }

    private static removeUserInfo() {
        if (localStorage.userInfo) {
            localStorage.removeItem('userInfo');
        }
    }
}
