import * as moment from 'moment';

export interface IUserLoginInfoDto {
    name: string | undefined;
    surname: string | undefined;
    userName: string | undefined;
    emailAddress: string | undefined;
    id: number | undefined;
}

export class ApplicationInfoDto implements IApplicationInfoDto {
    version: string | undefined;
    releaseDate: moment.Moment | undefined;
    features: { [key: string]: boolean; } | undefined;

    constructor(data?: IApplicationInfoDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.version = data["version"];
            this.releaseDate = data["releaseDate"] ? moment(data["releaseDate"].toString()) : <any>undefined;
            if (data["features"]) {
                this.features = {};
                for (let key in data["features"]) {
                    if (data["features"].hasOwnProperty(key))
                        this.features[key] = data["features"][key];
                }
            }
        }
    }

    static fromJS(data: any): ApplicationInfoDto {
        data = typeof data === 'object' ? data : {};
        let result = new ApplicationInfoDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["version"] = this.version;
        data["releaseDate"] = this.releaseDate ? this.releaseDate.toISOString() : <any>undefined;
        if (this.features) {
            data["features"] = {};
            for (let key in this.features) {
                if (this.features.hasOwnProperty(key))
                    data["features"][key] = this.features[key];
            }
        }
        return data;
    }

    clone(): ApplicationInfoDto {
        const json = this.toJSON();
        let result = new ApplicationInfoDto();
        result.init(json);
        return result;
    }
}

export interface IApplicationInfoDto {
    version: string | undefined;
    releaseDate: moment.Moment | undefined;
    features: { [key: string]: boolean; } | undefined;
}

export class UserLoginInfoDto implements IUserLoginInfoDto {
    name: string | undefined;
    surname: string | undefined;
    userName: string | undefined;
    emailAddress: string | undefined;
    id: number | undefined;

    constructor(data?: IUserLoginInfoDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.name = data["name"];
            this.surname = data["surname"];
            this.userName = data["userName"];
            this.emailAddress = data["emailAddress"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): UserLoginInfoDto {
        data = typeof data === 'object' ? data : {};
        let result = new UserLoginInfoDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["name"] = this.name;
        data["surname"] = this.surname;
        data["userName"] = this.userName;
        data["emailAddress"] = this.emailAddress;
        data["id"] = this.id;
        return data;
    }

    clone(): UserLoginInfoDto {
        const json = this.toJSON();
        let result = new UserLoginInfoDto();
        result.init(json);
        return result;
    }
}


export class TenantLoginInfoDto implements ITenantLoginInfoDto {
    tenancyName: string | undefined;
    name: string | undefined;
    id: number | undefined;

    constructor(data?: ITenantLoginInfoDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.tenancyName = data["tenancyName"];
            this.name = data["name"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): TenantLoginInfoDto {
        data = typeof data === 'object' ? data : {};
        let result = new TenantLoginInfoDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["tenancyName"] = this.tenancyName;
        data["name"] = this.name;
        data["id"] = this.id;
        return data;
    }

    clone(): TenantLoginInfoDto {
        const json = this.toJSON();
        let result = new TenantLoginInfoDto();
        result.init(json);
        return result;
    }
}

export interface ITenantLoginInfoDto {
    tenancyName: string | undefined;
    name: string | undefined;
    id: number | undefined;
}