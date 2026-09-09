import { html, css, when } from "@umbraco-cms/backoffice/external/lit";
import { UmbLitElement } from "@umbraco-cms/backoffice/lit-element";
import { UMB_PROPERTY_CONTEXT } from "@umbraco-cms/backoffice/property";

export function parseBoolean(value) {
    if (typeof value === "boolean") return value;
    if (typeof value === "number") return value !== 0;
    if (typeof value === "string") {
        const normalized = value.trim().toLowerCase();
        return normalized === "1" || normalized === "true";
    }
    return Boolean(value);
}

export class LimboMultiNodeTreePickerSeparatorElement extends UmbLitElement {

    _label = "";
    _description = "";

    set config(config) {
        this.first = parseBoolean(config?.getValueByAlias("first"));
    }

    constructor() {
        super();
        this.consumeContext(UMB_PROPERTY_CONTEXT, (ctx) => {
            if (!ctx) return;
            if (ctx.label) this.observe(ctx.label, (v) => (this._label = v ?? ""));
            if (ctx.description) this.observe(ctx.description, (v) => (this._description = v ?? ""));
        });
    }

    connectedCallback() {
        super.connectedCallback();
        const umbPropertyLayout = this.parentElement?.parentElement;
        if (umbPropertyLayout) {
            umbPropertyLayout.setAttribute("orientation", "vertical");
            const headerColumn = umbPropertyLayout.shadowRoot?.querySelector("#headerColumn");
            if (headerColumn) headerColumn.style.display = "none";
        }
    }

    render() {
        return html`
            <div class="limbo-separator ${this.first ? "first" : ""}">
                <div class="limbo-separator-title">${this._label}</div>
                ${when(this._description, () => html`
                    <div class="limbo-separator-description">${this._description}</div>
                `)}
            </div>
        `;
    }

    static styles = css`

        .limbo-separator {
            border-bottom: 2px solid #F4C1BC;
            margin: 0;
            padding: 0;
            margin-bottom: -21px;
            z-index: 2;
            position: relative;
            &.first {
                margin-top: -21px;
            }
        }

        .limbo-separator-title {
            font-size: 13px;
            text-transform: uppercase;
        }

        .limbo-separator-description {
            font-size: 11px;
            color: #333;
            margin-top: -5px;
        }

  `;

}

customElements.define("limbo-multi-node-tree-picker-separator", LimboMultiNodeTreePickerSeparatorElement);

export default LimboMultiNodeTreePickerSeparatorElement;