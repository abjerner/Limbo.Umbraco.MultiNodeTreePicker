import { html } from '@umbraco-cms/backoffice/external/lit';
import { UmbLitElement } from '@umbraco-cms/backoffice/lit-element';
import { UmbChangeEvent } from '@umbraco-cms/backoffice/event';
import { UmbFormControlMixin } from '@umbraco-cms/backoffice/validation';

import { umbExtensionsRegistry } from '@umbraco-cms/backoffice/extension-registry';

import {
    UmbPropertyEditorConfigCollection,
} from '@umbraco-cms/backoffice/property-editor';

export class LimboMultiNodeTreePickerElement extends UmbFormControlMixin(UmbLitElement) {

    static properties = {
        value: { attribute: false },
        config: { attribute: false }
    };

    async connectedCallback() {

        super.connectedCallback();

        // if the buil-in element hasn't been loaded yet, we need to find the manifest, and load the element ourselves
        if (!customElements.get("umb-property-editor-ui-content-picker")) {
            const manifest = umbExtensionsRegistry.getByAlias("Umb.PropertyEditorUi.ContentPicker");
            if (manifest?.element) await manifest.element();
        }

        console.log(this.config);
        console.log(this.config?.constructor?.name);
        console.log(typeof this.config?.getValueByAlias);
        console.log(this.config instanceof UmbPropertyEditorConfigCollection);

        console.log('startNode', this.config.getValueByAlias('startNode'));
        console.log('minNumber', this.config.getValueByAlias('minNumber'));
        console.log('maxNumber', this.config.getValueByAlias('maxNumber'));
        console.log('filter', this.config.getValueByAlias('filter'));
        console.log('startNode', this.config.getValueByAlias('startNode'));

    }

    firstUpdated() {
        const picker = this.shadowRoot.querySelector(
            'umb-property-editor-ui-content-picker'
        );

        console.log('parent config', this.config);
        console.log('child config', picker?.config);

        picker.config = this.config;
    }

    render() {
        return html`
            <umb-property-editor-ui-content-picker
                .value=${this.value}
                .config=${this.config}
                @change=${this.#onChange}>
            </umb-property-editor-ui-content-picker>
        `;
    }

    #onChange = (event) => {
        this.value = event.target.value;
        this.dispatchEvent(new CustomEvent("change"));
    };

}

customElements.define("limbo-multi-node-tree-picker", LimboMultiNodeTreePickerElement);

export default LimboMultiNodeTreePickerElement;