const LOGIN_PAGE_URL = '/Auth/Login'
const handle401Unauthorized = (response) => {
    window.location.href = LOGIN_PAGE_URL
}

const useRequest = function () {
    // NOTE: �Юھګ��API��m�վ�
    const BASE_URL = '/'
    const request = axios.create({ baseURL: BASE_URL })

    const beforeRequest = (config) => {
        // �o request �e�B�z

        // �p�G�� JWT Token �N�a
        const token = localStorage.getItem('token')
        token && (config.headers['Authorization'] = `Bearer ${token}`)

        return config
    }

    // �ШD�d�I��
    request.interceptors.request.use(beforeRequest)

    const responseSuccess = (response) => {
        // 2XX
        // NOTE: �Юھګ��API���f���վ�

        // console.log(response)
        return response.data
    }

    const responseFail = (err) => {
        // !2XX

        // console.log(err)
        const { response } = err
        const { statusText, status } = response

        // NOTE: �Τ@�B�z���Ѧ欰
        switch (status) {
            case 401:
                // TODO: handle 401 ex. redirect
                handle401Unauthorized(response)
                break
        }

        return Promise.reject(response)
    }

    // �^���d�I��
    request.interceptors.response.use(responseSuccess, responseFail)


    return {
        httpGet: request.get,
        httpPost: request.post,
        httpPut: request.put,
        httpDelete: request.delete,
    }
}

const {
    httpPost,
    httpGet,
    httpDelete,
    httpPut,
} = useRequest();
