/**
 * Tình trạng Form Employee
 */
export const FORM_STATE = {
    ADD: 0,
    EDIT: 1,
}

/**
 * Tình trạng Popup
 */
export const POPUP_STATE = {
    DELETE: 1,
    UPDATE_DATA: 0,
    DUPLICATED_UNIT: 2,
    DUPLICATED_CODE: 3,
}

/**
 * Trạng thái mã code
 */
export const STATUS_CODE = {
    SUCCESS: 200,
    NO_CONTENT: 204,
    BAD_REQUEST: 400,
    ERROR_SERVER: 500,
    NOT_FOUND: 404,
    CREATED: 201,
}

//Link URL InventoryItem
export const URL = "https://localhost:44337/api/v1/InventoryItems"

//Link URL InventoryItemUnitConvert
export const URL_UnitConvert = "https://localhost:44337/api/v1/InventoryItemUnitConverts"

//Link URL Unit
export const URL_Unit = "https://localhost:44337/api/v1/Units"

//Link URL Stock
export const URL_Stock = "https://localhost:44337/api/v1/Stocks"