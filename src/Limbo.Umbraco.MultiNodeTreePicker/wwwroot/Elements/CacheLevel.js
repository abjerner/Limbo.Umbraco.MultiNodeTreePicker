import { css, html } from "@umbraco-cms/backoffice/external/lit";
import { UmbLitElement } from "@umbraco-cms/backoffice/lit-element";
import { UmbChangeEvent } from "@umbraco-cms/backoffice/event";

/**
 * The values match the "Umbraco.Cms.Core.PropertyEditors.PropertyCacheLevel" enum. "Snapshot" is no longer offered as
 * the published cache no longer supports snapshotting as of Umbraco 15 - note that the enum member itself still
 * exists, so a data type upgraded from an older version of the package can still hold that value.
 */
const LEVELS = [
	{
		value: "Element",
		name: "Element",
		description: "The property value can be cached until the element itself is modified.",
	},
	{
		value: "Elements",
		name: "Elements",
		description: "The property value can be cached until any element is modified. This is the default.",
	},
	{
		value: "None",
		name: "None",
		description: "The property value cannot be cached and has to be converted each time it is requested.",
	},
];

const DEFAULT_LEVEL = "Elements";

export class LimboMultiNodeTreePickerCacheLevelElement extends UmbLitElement {

	static properties = {
		value: { attribute: false },
		readonly: { type: Boolean, reflect: true },
	};

	static styles = [
		css`
			:host {
				display: block;
			}
			#buttons {
				display: flex;
				flex-wrap: wrap;
				gap: var(--uui-size-space-2);
			}
			#description {
				color: var(--uui-color-text-alt);
				font-size: var(--uui-type-small-size);
				margin-top: var(--uui-size-space-2);
			}
		`,
	];

	constructor() {
		super();
		this.readonly = false;
	}

	/**
	 * Note that a stored value that isn"t one of the levels above - eg. the "Snapshot" level offered by older versions
	 * of the package - is deliberately returned as-is. Falling back to the default would highlight "Elements" while
	 * the data type is in fact still configured with the stored level.
	 */
	get #selected() {
		return this.value ?? DEFAULT_LEVEL;
	}

	#onClick(level) {
		this.value = level.value;
		this.dispatchEvent(new UmbChangeEvent());
	}

	render() {

		const selected = this.#selected;
		const known = LEVELS.find((x) => x.value === selected);

		return html`
			<div id="buttons">
				${LEVELS.map(
			// The closing tag must follow the opening tag immediately. "uui-button" only renders its "label"
			// property when its default slot is empty, and it counts a whitespace-only text node - which is
			// exactly what a line break before the closing tag produces - as slotted content.
			(level) => html`<uui-button
							look=${level.value === selected ? "primary" : "outline"}
							color=${level.value === selected ? "positive" : "default"}
							label=${level.name}
							title=${level.description}
							?disabled=${this.readonly}
							@click=${() => this.#onClick(level)}></uui-button>`,
		)}
			</div>
			<div id="description">
				${known
				? known.description
				: `The data type is configured with the cache level "${selected}", which is no longer offered. Pick one of the levels above to change it.`}
			</div>
		`;

	}

}

customElements.define("limbo-multi-node-tree-picker-cache-level", LimboMultiNodeTreePickerCacheLevelElement);

export default LimboMultiNodeTreePickerCacheLevelElement;