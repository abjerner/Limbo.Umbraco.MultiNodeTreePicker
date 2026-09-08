
import { umbHttpClient } from "@umbraco-cms/backoffice/http-client";
import { tryExecute } from "@umbraco-cms/backoffice/resources";


import { LIMBO_MNTP_API_PATH } from "@limbo/mntp/constants";

export class MntpService {

	static async getConverters() {
		return umbHttpClient.get({
			url: `${LIMBO_MNTP_API_PATH}/converters`,
			security: [{ type: "http", scheme: "bearer" }],
		});
	}

}

export default MntpService;