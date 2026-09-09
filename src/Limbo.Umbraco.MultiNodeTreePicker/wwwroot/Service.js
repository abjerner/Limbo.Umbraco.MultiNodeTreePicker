import { umbHttpClient } from "@umbraco-cms/backoffice/http-client";
import { tryExecute } from "@umbraco-cms/backoffice/resources";

import { LIMBO_MNTP_API_PATH } from "@limbo/mntp/constants";

/**
 * Thin wrapper around the package's Management API. Requests go through Umbraco's authenticated HTTP client, so the
 * bearer token and error handling are taken care of by the backoffice.
 */
export class MntpService {

	/**
	 * Returns the converters (type converters and item converters) registered on the server.
	 * @param {import("@umbraco-cms/backoffice/controller-api").UmbControllerHost} host The host element, used for notifications on failure.
	 * @returns {Promise<{ data?: Array<{ type: string, name: string, icon: string, description: string, assembly?: string }>, error?: unknown }>}
	 */
	static getConverters(host) {
		return tryExecute(host, umbHttpClient.get({
			url: `${LIMBO_MNTP_API_PATH}/converters`,
			security: [{ type: "http", scheme: "bearer" }],
		}));
	}

}

export default MntpService;
