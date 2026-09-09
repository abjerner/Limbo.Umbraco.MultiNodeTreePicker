import { html, css, nothing } from "@umbraco-cms/backoffice/external/lit";
import { UmbLitElement } from "@umbraco-cms/backoffice/lit-element";
import { UmbChangeEvent } from "@umbraco-cms/backoffice/event";
import { UMB_ITEM_PICKER_MODAL, umbOpenModal } from "@umbraco-cms/backoffice/modal";

import { MntpService } from "@limbo/mntp/service";

/**
 * Normalises the different value shapes the package has stored over time into `{ type }`.
 *
 * - v17: `{ type: "Namespace.Type, Assembly" }`
 * - v13: `{ type: "Namespace.Type, Assembly" }` or a plain string
 * - very early versions: `{ key: "Namespace.Type, Assembly, Version=..." }`
 *
 * @param {unknown} value
 * @returns {{ type: string } | undefined}
 */
function normalize(value) {

	let type;

	if (typeof value === "string") {
		type = value;
	} else if (value && typeof value === "object") {
		type = value.type ?? value.key;
	}

	if (typeof type !== "string") return undefined;

	// Strip assembly version information, so only "Namespace.Type, Assembly" is left
	type = type.split(",").slice(0, 2).map((x) => x.trim()).join(", ");

	return type ? { type } : undefined;

}

/**
 * Property editor UI for selecting the type converter (or item converter) of a Limbo multinode treepicker data type.
 *
 * @element limbo-mntp-type-converter
 */
export class LimboMntpTypeConverterElement extends UmbLitElement {

	static properties = {
		config: { attribute: false },
		readonly: { type: Boolean, reflect: true },
		_loading: { state: true },
		_converters: { state: true },
		_selected: { state: true },
		_notFound: { state: true },
	};

	#value;

	constructor() {
		super();
		this.readonly = false;
		this._loading = true;
		this._converters = [];
		this._selected = undefined;
		this._notFound = false;
	}

	set value(value) {
		const oldValue = this.#value;
		this.#value = normalize(value);
		this.#resolveSelected();
		this.requestUpdate("value", oldValue);
	}

	get value() {
		return this.#value;
	}

	async connectedCallback() {

		super.connectedCallback();

		if (!this._loading) return;

		const { data } = await MntpService.getConverters(this);

		this._converters = Array.isArray(data) ? data : [];
		this._loading = false;

		this.#resolveSelected();

	}

	#resolveSelected() {

		if (this._loading) return;

		if (!this.#value?.type) {
			this._selected = undefined;
			this._notFound = false;
			return;
		}

		const type = this.#value.type.toLowerCase();

		this._selected = this._converters.find((x) => x.type.toLowerCase() === type);
		this._notFound = !this._selected;

	}

	async #onPick() {

		if (this.readonly) return;

		let picked;

		try {
			picked = await umbOpenModal(this, UMB_ITEM_PICKER_MODAL, {
				data: {
					headline: "Select type converter",
					items: this._converters.map((x) => ({
						label: x.name,
						description: x.description,
						icon: x.icon,
						value: x.type,
					})),
				},
			});
		} catch {
			// The modal was closed without a selection
			return;
		}

		if (!picked?.value) return;

		this.value = { type: picked.value };
		this.dispatchEvent(new UmbChangeEvent());

	}

	#onRemove() {
		if (this.readonly) return;
		this.value = undefined;
		this.dispatchEvent(new UmbChangeEvent());
	}

	render() {
		if (this._loading) return html`<uui-loader-bar></uui-loader-bar>`;
		if (this._selected) return this.#renderSelected();
		if (this._notFound) return this.#renderNotFound();
		return this.#renderAddButton();
	}

	#renderSelected() {
		return html`
			<uui-ref-node standalone name=${this._selected.name} detail=${this._selected.description ?? ""} ?readonly=${this.readonly}>
				<umb-icon slot="icon" name=${this._selected.icon}></umb-icon>
				${this.readonly ? nothing : html`
					<uui-action-bar slot="actions">
						<uui-button @click=${this.#onPick} label=${this.localize.term("general_change")}></uui-button>
						<uui-button @click=${this.#onRemove} label=${this.localize.term("general_remove")}></uui-button>
					</uui-action-bar>
				`}
			</uui-ref-node>
		`;
	}

	#renderNotFound() {
		return html`
			<div id="not-found">
				<p>
					The selected type converter <strong>${this.#value.type}</strong> could not be found.
					Make sure the assembly is still referenced, or select another converter.
				</p>
				${this.readonly ? nothing : html`
					<uui-button look="secondary" @click=${this.#onPick} label=${this.localize.term("general_change")}></uui-button>
					<uui-button look="secondary" color="danger" @click=${this.#onRemove} label=${this.localize.term("general_remove")}></uui-button>
				`}
			</div>
		`;
	}

	#renderAddButton() {
		if (this.readonly) return nothing;
		if (!this._converters.length) {
			return html`<p id="empty">There are no type converters available.</p>`;
		}
		return html`
			<uui-button
				id="add-button"
				look="placeholder"
				@click=${this.#onPick}
				label=${this.localize.term("general_add")}></uui-button>
		`;
	}

	static styles = [
		css`
			:host {
				display: block;
			}

			#add-button {
				width: 100%;
			}

			#not-found {
				padding: var(--uui-size-space-4);
				border: 1px solid var(--uui-color-danger-standalone);
				border-radius: var(--uui-border-radius);
				color: var(--uui-color-danger-standalone);
				background: var(--uui-color-danger-surface, transparent);
			}

			#not-found p {
				margin: 0 0 var(--uui-size-space-3) 0;
			}

			#empty {
				margin: 0;
				color: var(--uui-color-text-alt);
			}
		`,
	];

}

customElements.define("limbo-mntp-type-converter", LimboMntpTypeConverterElement);

export default LimboMntpTypeConverterElement;
