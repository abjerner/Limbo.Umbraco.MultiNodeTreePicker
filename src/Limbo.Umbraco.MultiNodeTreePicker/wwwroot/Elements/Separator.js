import { html, css } from "@umbraco-cms/backoffice/external/lit";
import { UmbLitElement } from "@umbraco-cms/backoffice/lit-element";

/**
 * Property editor UI that stores nothing and only renders a horizontal rule. Used together with the property label
 * to visually group the settings of the Limbo multinode treepicker into sections.
 *
 * @element limbo-mntp-separator
 */
export class LimboMntpSeparatorElement extends UmbLitElement {

	static properties = {
		value: { attribute: false },
		config: { attribute: false },
	};

	render() {
		return html`<hr />`;
	}

	static styles = [
		css`
			:host {
				display: block;
			}

			hr {
				margin: var(--uui-size-space-3) 0 0 0;
				border: 0;
				border-top: 1px solid var(--uui-color-border);
			}
		`,
	];

}

customElements.define("limbo-mntp-separator", LimboMntpSeparatorElement);

export default LimboMntpSeparatorElement;
