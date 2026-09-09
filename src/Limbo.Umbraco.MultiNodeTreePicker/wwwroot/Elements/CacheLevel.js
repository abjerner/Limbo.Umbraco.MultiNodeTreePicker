import { html, css, repeat } from "@umbraco-cms/backoffice/external/lit";
import { UmbLitElement } from "@umbraco-cms/backoffice/lit-element";
import { UmbChangeEvent } from "@umbraco-cms/backoffice/event";

const LEVELS = [
	{
		alias: "Element",
		name: "Element",
		description: "The property value is cached at the element level, i.e. until the element itself is modified.",
	},
	{
		alias: "Elements",
		name: "Elements",
		description: "The property value is cached at the elements level, i.e. until any element is modified. This is the default.",
	},
	{
		alias: "None",
		name: "None",
		description: "The property value is not cached and is converted each time it is requested.",
	},
];

const DEFAULT_LEVEL = "Elements";

/**
 * Maps a stored value to one of the supported levels. "Snapshot" (from v13) no longer exists and maps to "Elements".
 * @param {unknown} value
 * @returns {string}
 */
function normalize(value) {
	if (typeof value !== "string") return DEFAULT_LEVEL;
	const match = LEVELS.find((x) => x.alias.toLowerCase() === value.trim().toLowerCase());
	return match?.alias ?? DEFAULT_LEVEL;
}

/**
 * Property editor UI for selecting the cache level of the Limbo multinode treepicker's property value converter.
 *
 * @element limbo-mntp-cache-level
 */
export class LimboMntpCacheLevelElement extends UmbLitElement {

	static properties = {
		config: { attribute: false },
		readonly: { type: Boolean, reflect: true },
		_selected: { state: true },
	};

	#value;

	constructor() {
		super();
		this.readonly = false;
		this._selected = DEFAULT_LEVEL;
	}

	set value(value) {
		const oldValue = this.#value;
		this.#value = value;
		this._selected = normalize(value);
		this.requestUpdate("value", oldValue);
	}

	get value() {
		return this.#value;
	}

	#onSelect(level) {
		if (this.readonly) return;
		if (this._selected === level.alias) return;
		this.value = level.alias;
		this.dispatchEvent(new UmbChangeEvent());
	}

	render() {
		return html`
			<uui-button-group>
				${repeat(LEVELS, (level) => level.alias, (level) => html`
					<uui-button
						look=${this._selected === level.alias ? "primary" : "secondary"}
						label=${level.name}
						title=${level.description}
						?disabled=${this.readonly}
						@click=${() => this.#onSelect(level)}>
						${level.name}
					</uui-button>
				`)}
			</uui-button-group>
		`;
	}

	static styles = [
		css`
			:host {
				display: block;
			}
		`,
	];

}

customElements.define("limbo-mntp-cache-level", LimboMntpCacheLevelElement);

export default LimboMntpCacheLevelElement;
