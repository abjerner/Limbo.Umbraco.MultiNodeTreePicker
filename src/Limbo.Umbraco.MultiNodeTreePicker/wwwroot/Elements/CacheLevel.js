import { css, html, repeat, when } from "@umbraco-cms/backoffice/external/lit";
import { UmbLitElement } from "@umbraco-cms/backoffice/lit-element";
import { UmbChangeEvent } from "@umbraco-cms/backoffice/event";

/**
 * The values match the "Umbraco.Cms.Core.PropertyEditors.PropertyCacheLevel" enum. "Snapshot" is no longer offered as
 * the published cache no longer supports snapshotting as of Umbraco 15 - note that the enum member itself still
 * exists, so a data type upgraded from an older version of the package can still hold that value.
 */
const LEVELS = [
	{
		alias: "Element",
		name: "Element",
		description: "The property value can be cached until the element itself is modified.",
	},
	{
		alias: "Elements",
		name: "Elements",
		description: "The property value can be cached until any element is modified. This is the default.",
	},
	{
		alias: "None",
		name: "None",
		description: "The property value cannot be cached and has to be converted each time it is requested.",
	},
];

const DEFAULT_LEVEL = "Elements";

function getCacheLevel(alias) {
	return (alias ? LEVELS.find((x) => x.alias.toLowerCase() === alias.trim().toLowerCase()) : null) ?? LEVELS.find((x) => x.alias === DEFAULT_LEVEL);
}

/**
 * Property editor UI for selecting the cache level of the Limbo multinode treepicker's property value converter.
 *
 * @element limbo-mntp-cache-level
 */
export class LimboMultiNodeTreePickerCacheLevelElement extends UmbLitElement {

	static properties = {
		value: { attribute: false },
		readonly: { type: Boolean, reflect: true },
		_selected: { state: true }
	};

	#value;

	set value(value) {
		const oldValue = this.#value;
		this.#value = value;
		this._selected = getCacheLevel(value);
		this.requestUpdate("value", oldValue);
	}

	get value() {
		return this.#value;
	}

	constructor() {
		super();
		this.readonly = false;
		this._selected = getCacheLevel(DEFAULT_LEVEL);
	}

	#onSelect(level) {
		if (this.readonly) return;
		if (this._selected?.alias === level.alias) return;
		this.value = level.alias;
		this.dispatchEvent(new UmbChangeEvent());
	}

	render() {
		return html`
			<div id="buttons">
				${repeat(LEVELS, (level) => level.alias, (level) => html`
					<uui-button
						look=${this._selected?.alias === level.alias ? "primary" : "outline"}
						color=${this._selected?.alias === level.alias ? "positive" : "default"}
						label=${level.name}
						title=${level.description}
						?disabled=${this.readonly}
						@click=${() => this.#onSelect(level)}>
						${level.name}
					</uui-button>`
		)}
			</div>
			<div id="description">
				${this._selected?.description
				? this._selected?.description
				: `The data type is configured with the cache level "${this._selected?.alias}", which is no longer offered. Pick one of the levels above to change it.`
			}
			</div>
		`;
	}

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

}

customElements.define("limbo-multi-node-tree-picker-cache-level", LimboMultiNodeTreePickerCacheLevelElement);

export default LimboMultiNodeTreePickerCacheLevelElement;