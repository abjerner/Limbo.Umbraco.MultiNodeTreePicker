import { html, nothing } from "@umbraco-cms/backoffice/external/lit";
import { UmbLitElement } from "@umbraco-cms/backoffice/lit-element";
import { UmbChangeEvent } from "@umbraco-cms/backoffice/event";
import { UmbFormControlMixin } from "@umbraco-cms/backoffice/validation";

import { umbExtensionsRegistry } from "@umbraco-cms/backoffice/extension-registry";
import { loadManifestElement } from "@umbraco-cms/backoffice/extension-api";

import { UMB_CONTENT_PICKER_UI_ALIAS } from "@limbo/mntp/constants";

const INNER_TAG = "umb-property-editor-ui-content-picker";

/**
 * Property editor UI for the Limbo multinode treepicker.
 *
 * The element is a thin wrapper around Umbraco's built-in content picker UI. All picking behaviour (tree, start
 * node, dynamic roots, min/max validation, allowed types) is delegated to the built-in element; this wrapper only
 * exists so the Limbo schema can be selected as its own property editor in the backoffice.
 *
 * @element limbo-mntp
 */
export class LimboMultiNodeTreePickerElement extends UmbFormControlMixin(UmbLitElement) {

	static properties = {
		value: { attribute: false },
		config: { attribute: false },
		readonly: { type: Boolean, reflect: true },
		mandatory: { type: Boolean },
		mandatoryMessage: { type: String },
		_ready: { state: true }
	};

	#registered = false;

	set value(value) {
		const oldValue = super.value;
		super.value = value;
		this.requestUpdate("value", oldValue);
	}

	get value() {
		return super.value;
	}

	constructor() {
		super();
		this.readonly = false;
		this.mandatory = false;
		this.mandatoryMessage = undefined;
		this._ready = customElements.get(INNER_TAG) !== undefined;
	}

	async connectedCallback() {
		super.connectedCallback();
		await this.#ensureInnerElement();
	}

	/**
	 * The built-in content picker element is lazy loaded by the backoffice, so it may not be defined yet when this
	 * element is first rendered. If that is the case, we load it through its manifest before rendering.
	 */
	async #ensureInnerElement() {

		if (customElements.get(INNER_TAG)) {
			this._ready = true;
			return;
		}

		const manifest = umbExtensionsRegistry.getByAlias(UMB_CONTENT_PICKER_UI_ALIAS);

		if (manifest?.element) {
			await loadManifestElement(manifest.element);
		}

		await customElements.whenDefined(INNER_TAG);

		this._ready = true;

	}

	firstUpdated(changedProperties) {
		super.firstUpdated?.(changedProperties);
		this.#registerInnerFormControl();
	}

	updated(changedProperties) {
		super.updated?.(changedProperties);
		if (changedProperties.has("_ready") && this._ready) this.#registerInnerFormControl();
	}

	#registerInnerFormControl() {
		const inner = this.shadowRoot?.querySelector(INNER_TAG);
		if (inner && !this.#registered) {
			this.#registered = true;
			this.addFormControlElement(inner);
		}
	}

	#onChange = (event) => {
		event.stopPropagation();
		this.value = event.target.value;
		this.dispatchEvent(new UmbChangeEvent());
	};

	render() {
		if (!this._ready) return nothing;
		return html`
            <umb-property-editor-ui-content-picker
				.value=${this.value}
				.config=${this.config}
				?readonly=${this.readonly}
				.mandatory=${this.mandatory}
				.mandatoryMessage=${this.mandatoryMessage}
				@change=${this.#onChange}>
            </umb-property-editor-ui-content-picker>
        `;
	}

}

customElements.define("limbo-multi-node-tree-picker", LimboMultiNodeTreePickerElement);

export default LimboMultiNodeTreePickerElement;